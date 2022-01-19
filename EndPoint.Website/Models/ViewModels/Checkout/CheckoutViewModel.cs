using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Application.Services.Address.Queries.IGetAddresses;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Models.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public CartDto Cart { get; set; }
        public AddressesDto Addresses { get; set; }

    }
}
