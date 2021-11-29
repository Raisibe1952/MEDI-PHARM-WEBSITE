using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.Modules;
using Medipharmacy.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medipharmacy.Controllers
{
    public class CartController : Controller
    {
        private DatabaseContext db;

        public CartController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet("cart/view")]
        public IActionResult ViewCart()
        {
            var userId = Module.CheckCustomer(HttpContext);

            if (userId == 0)
            {
                ViewBag.message = "You must be signed in to view cart!";
                return PartialView("SigninPartial");
            }

            try
            {
                var cart = db.Cart.Where(c => c.UserId.Equals(userId) && c.Checkedout.Equals(false)).ToList();
                var products = new List<Product>();
                var prices = new List<Price>();
                foreach(var item in cart)
                {
                    var product = db.Product.Find(item.ProductId);

                    if (product != null)
                    {
                        products.Add(product);
                    }
                    else
                    {
                        products.Add(new Product(item.ProductId, "", 0, 0, "No Name", "", 0, 0, 0, DateTime.Now, DateTime.Now, "~/img/logo.svg"));
                    }

                    var price = db.Price.Find(item.PriceId);
                    if (price != null)
                    {
                        prices.Add(price);
                    }
                    else
                    {
                        prices.Add(new Price(item.PriceId, item.ProductId, 0.0, false, DateTime.Now));
                    }
                }
                ViewBag.products = products;
                ViewBag.prices = prices;
                return PartialView(cart);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }

        [HttpGet("cart/add/{productId}")]
        public IActionResult AddToCart(int productId)
        {
            var userId = Module.CheckCustomer(HttpContext);

            if (userId == 0)
            {
                ViewBag.message = "You must be signed in to add a product!";
                return PartialView("SigninPartial");
            }

            try
            {
                var cart = db.Cart.Where(c => c.UserId.Equals(userId) && c.ProductId.Equals(productId) && c.Checkedout.Equals(false)).First();
                cart.Quantity++;
                db.Cart.Update(cart);
                db.SaveChanges();
            }
            catch (Exception)
            {
                try
                {
                    var priceId = db.Price.Where(p => p.ProductId.Equals(productId)).OrderByDescending(p => p.DateCreated).First().PriceId;
                    var cart = new Cart(0, userId, productId, 1, priceId, false);
                    db.Cart.Add(cart);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return PartialView("StatusPartial", new StatusViewModel(Module.Status.FAILURE, "could not add product..."));
                }
            }
            return PartialView("StatusPartial", new StatusViewModel(Module.Status.SUCCESS, "added to cart..."));
        }

        [HttpGet("cart/minus/{cartId}")]
        public IActionResult Minus(int cartId)
        {
            var userId = Module.CheckCustomer(HttpContext);

            if (userId == 0)
            {
                ViewBag.message = "You must be signed in to change quantity!";
                return PartialView("SigninPartial");
            }

            var cart = db.Cart.Find(cartId);

            if (cart != null)
            {
                cart.Quantity--;
                db.Cart.Update(cart);
                db.SaveChanges();
            }

            return Redirect("/cart/view");
        }

        [HttpGet("cart/plus/{cartId}")]
        public IActionResult Plus(int cartId)
        {
            var userId = Module.CheckCustomer(HttpContext);

            if (userId == 0)
            {
                ViewBag.message = "You must be signed in to change quantity!";
                return PartialView("SigninPartial");
            }

            var cart = db.Cart.Find(cartId);

            if (cart != null)
            {
                cart.Quantity++;
                db.Cart.Update(cart);
                db.SaveChanges();
            }

            return Redirect("/cart/view");
        }

        [HttpGet("cart/delete/{cartId}")]
        public IActionResult Delete(int cartId)
        {
            var userId = Module.CheckCustomer(HttpContext);

            if (userId == 0)
            {
                ViewBag.message = "You must be signed in to delete a product!";
                return PartialView("SigninPartial");
            }

            var cart = db.Cart.Find(cartId);

            if(cart != null)
            {
                db.Cart.Remove(cart);
                db.SaveChanges();
                ViewBag.message = "Product deleted...";
            }

            return Redirect("/cart/view");
        }

        [HttpGet("cart/remove/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = Module.CheckCustomer(HttpContext);

            if (userId == 0)
            {
                ViewBag.message = "You must be signed in to remove a product!";
                return PartialView("SigninPartial");
            }

            try
            {
                var cart = db.Cart.Where(c => c.UserId.Equals(userId) && c.ProductId.Equals(productId)).First();
                db.Cart.Remove(cart);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.message = "Product could not be removed from cart!";
            }

            try
            {
                var cart_list = db.Cart.Where(c => c.UserId.Equals(userId) && c.Checkedout.Equals(false)).ToList();
                return PartialView("ViewCart", cart_list);
            }
            catch (Exception)
            {
                return PartialView("ViewCart");
            }
        }
    }
}
