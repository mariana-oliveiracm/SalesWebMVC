using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
            //var sellersList = _sellerService.FindAll();
            var sellersList = await _sellerService.FindAllAsync();
            return View(sellersList);
        }

        public async Task<IActionResult> Create()
        {
            //var departments = _departmentService.FindAll();
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                //var departments = _departmentService.FindAll();
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            try
            {
                //_sellerService.Insert(seller);
                await _sellerService.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" }); ;
            //var seller = _sellerService.FindById(id.Value);
            var seller = await _sellerService.FindByIdAsync(id.Value);
            if (seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" }); ;
            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {            
            try
            {
                //_sellerService.Remove(id);
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            //catch (NotFoundException e)
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            /*catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }*/
            /*catch (DbConcurrencyException)
            {
                return BadRequest();
            }*/
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            //var seller = _sellerService.FindById(id.Value);
            var seller = await _sellerService.FindByIdAsync(id.Value);
            if (seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });
            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            //var seller = _sellerService.FindById(id.Value);
            var seller = await _sellerService.FindByIdAsync(id.Value);
            if (seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });
            //List<Department> departments = _departmentService.FindAll();
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (id != seller.Id) return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            if (!ModelState.IsValid)
            {
                //var departments = _departmentService.FindAll();
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            try
            {
                //_sellerService.Update(seller);
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error (string message)
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
