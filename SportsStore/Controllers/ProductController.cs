﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult List(string category, int productPage=1) => 
            View(
                new ProductListViewModel
                {
                    Products = repository
                        .Products
                        .Where(p=> category == null || p.Category == category)
                        .OrderBy(p => p.ProductId)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.Products.Count(p => category == null || p.Category == category)
                    },
                    CurrentCategory = category
                });
    }
}