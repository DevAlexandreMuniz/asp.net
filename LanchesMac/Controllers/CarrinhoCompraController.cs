using LanchesMac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LanchesMac.ViewModels;
using LanchesMac.Models;

namespace LanchesMac.Controllers;

public class CarrinhoCompraController : Controller
{
    private readonly ILancheRepository _lancheRepository;
    private readonly CarrinhoCompra _carrinhoCompra;

    public CarrinhoCompraController(ILancheRepository lancheRepository, CarrinhoCompra carrinhoCompra)
    {
        _lancheRepository = lancheRepository;
        _carrinhoCompra = carrinhoCompra;
    }

    public IActionResult Index()
    {
        var itens = _carrinhoCompra.GetCarrinhoCompraItens();
        _carrinhoCompra.CarrinhoCompraItems = itens;

        var carrinhoCompraVM = new CarrinhoCompraViewModel
        {
            CarrinhoCompra = _carrinhoCompra,
            CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
        };
        return View(carrinhoCompraVM);
    } 

    public RedirectToActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
    {
        var lancheSelecionado= _lancheRepository.Lanches.FirstOrDefault(p => p.LancheId == lancheId);

        if(lancheSelecionado != null)
        {
            _carrinhoCompra.AdicionarAoCarrinho(lancheSelecionado);
        }

        return RedirectToAction("index");
    }

    public IActionResult RemoverItemDoCarrinhoCompra(int lancheId)
    {
        var lancheSelecionado= _lancheRepository.Lanches.FirstOrDefault(p => p.LancheId == lancheId);

        if(lancheSelecionado != null)
        {
            _carrinhoCompra.RemoverDoCarrinho(lancheSelecionado);
        }

        return RedirectToAction("index");
    }
}