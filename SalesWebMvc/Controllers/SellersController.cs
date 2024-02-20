using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //Declarando uma dependencia para SellerService
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) 
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        // Monta a lista de vendedores
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }
        // Ação de inclusão de vendedor
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var ViewModel = new SellerFormViewModel { Departments = departments };
            return View(ViewModel);
        }
        [HttpPost] //annotation
        [ValidateAntiForgeryToken] //annotation
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var ViewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(ViewModel);
            }
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        // Ação de exclusão de vendedor
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }
            return View(obj);
        }
        
        [HttpPost] //annotation
        [ValidateAntiForgeryToken] //annotation
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        // Ação de detalhes do cadastro de vendedor
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }
            return View(obj);
        }

        // Ação de edição do cadastro de vendendor
        public IActionResult Edit(int? id)
        {
            if (id == null) // Testa se o id existe
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }            
            var obj = _sellerService.FindById(id.Value);
            if (obj == null) // Testa se o id é nulo
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }

            //Abre a tela de edição - instancia a ViewModel
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        [HttpPost] //annotation
        [ValidateAntiForgeryToken] //annotation
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var ViewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(ViewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch!" });
            }

            // Faz o tratamento de exceções
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }
        // Ação de error
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
