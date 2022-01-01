using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using AppMvcBasica.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DevIO.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, 
                                  IFornecedorRepository fornecedorRepository, 
                                  IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var ProdutoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(ProdutoViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return RedirectToAction(actionName: "Index");
        }
        
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            //capturar dados de uma instancia diferente da que veio pelo parametro produtoViewModel, para ter os dados separados que veio via formulario e dos dados do banco
            var produtoAtualizacao = await ObterProduto(id);
            //aqui eu repasso o que veio do banco sem precisar ir no banco de novo
            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;
            //se a img for atualizada, aqui nós tbm repassamos ela
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (!ModelState.IsValid) return View(produtoViewModel);

            //validar se a img foi preenchida, e se realmente tenho a intençao de realizar um novo upload de img
            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }
            
                produtoAtualizacao.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            //uma tecnica segura para edicao, pq evitamos que na hora de editar a pessoa force a editacao de campos que vc nao queira, como por exemplo, atribuir um novo registro no campo fornecedor para o produto, que combinamos ser nossa regra de negócio nao poder fazer
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            //passamos para fazer o mapeamento o produtoAtualizacao
            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await ObterProduto(id);
            
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }

            await _produtoRepository.Remover(id);

            return RedirectToAction("Index");
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));

            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            //aqui fazemos a verificação se o arquivo da img já existe na nossa pasta wwwroot, se sim, encerramos a execução do método e exibimos uma msg
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(key: string.Empty, errorMessage: "Já existe um arquivo com este nome!");
                return false;
            }

            //se tudo estiver ok, aqui a gravação do arquivo no disco
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
