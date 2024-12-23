﻿using Fricks.Repository.Commons;
using Fricks.Repository.Commons.Filters;
using Fricks.Service.BusinessModel.BrandModels;
using Fricks.Service.BusinessModel.CategoryModels;
using Fricks.Service.BusinessModel.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fricks.Service.Services.Interface
{
    public interface IProductService
    {
        //public Task<ProductModel> AddProduct(ProductRegisterModel product);
        public Task<ProductModel> AddProduct(CreateProductModel product, string email);
        public Task<ProductModel> UpdateProductInfo(UpdateProductModel productModel);
        public Task<ProductModel> DeleteProduct(int id);
        public Task<ProductModel> GetProductById(int id);
        public Task<Pagination<ProductModel>> GetAllProductByStoreIdPagination(int storeId, int brandId, int categoryId, PaginationParameter paginationParameter);
        public Task<Pagination<ProductListModel>> GetAllProductPagination(PaginationParameter paginationParameter, 
            ProductFilter productFilter, string currentEmail);

        public Task<bool> AddListProduct(List<CreateProductModel> productModels, string email);
    }
}
