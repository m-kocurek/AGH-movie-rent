﻿using AGH_movie_rent.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AGH_movie_rent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login( string returnUrl )
        {
            ViewData["ReturnUrl"] = returnUrl;  
            return View();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Validate( string username, string password, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (username == "bob" && password == "pizza")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier,username));
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme );
                var claimsPrincipal = new ClaimsPrincipal( claimsIdentity );
                await HttpContext.SignInAsync( claimsPrincipal);
                return Redirect(returnUrl);
            }
            TempData["Error"] = "Erorr Username or Password is invalid"; 

            return View("login"); 
        }

        [Authorize]
        public async Task<IActionResult> Logout() 
        { 
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
