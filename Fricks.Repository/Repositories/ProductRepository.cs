﻿using Fricks.Repository.Commons;
using Fricks.Repository.Commons.Filters;
using Fricks.Repository.Entities;
using Fricks.Repository.Migrations;
using Fricks.Repository.Repositories.Interface;
using Fricks.Repository.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fricks.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private FricksContext _context;
        private readonly DbSet<Product> _dbSet;
        public ProductRepository(FricksContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.Brand).Include(x => x.Category)
                                          .Include(x => x.ProductPrices).ThenInclude(x => x.Unit)
                                          .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductBySKUAsync(string sku)
        {
            return await _context.Products.Include(x => x.Brand).Include(x => x.Category)
                                          .Include(x => x.ProductPrices).ThenInclude(x => x.Unit)
                                          .Where(x => x.Sku == sku).FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetProductByStoreIdPaging(int id, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Products.CountAsync();
            var items = await _context.Products.Include(x => x.Brand).Include(x => x.Category)
                                    .Include(x => x.ProductPrices).ThenInclude(x => x.Unit)
                                    .Where(x => x.StoreId.Equals(id))
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Product>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        public async Task<Pagination<Product>> GetProductByStoreIdPaging(Brand? brand, Category? category, int id, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Products.CountAsync();
            var items = await _context.Products.Include(x => x.Brand).Include(x => x.Category)
                                    .Include(x => x.ProductPrices).ThenInclude(x => x.Unit)
                                    .Where(x => x.StoreId.Equals(id))
                                    .Where(x => brand != null ? brand.Equals(x.Brand) : x.Brand != null 
                                             && category != null ? category.Equals(x.Category) : x.Category != null)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Product>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.Store)
                                          .Include(x => x.ProductPrices).ThenInclude(x => x.Unit).ToListAsync();
        }

        public async Task<Pagination<Product>> GetProductPagingAsync(PaginationParameter paginationParameter, ProductFilter productFilter)
        {
            var query = _context.Products.Include(x => x.Store)
                                    .Include(x => x.Brand)
                                    .Include(x => x.Category)
                                    .Include(x => x.ProductPrices).ThenInclude(x => x.Unit)
                                    .Where(x => x.IsDeleted == false).AsQueryable();

            // apply filter
            query = ApplyProductFiltering(query, productFilter);

            var itemCount = await query.CountAsync();
            var items = await query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Product>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        public async Task<Pagination<Product>> GetProductPaging(Brand? brand, Category? category, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Products.CountAsync();
            var items = await _context.Products.Include(x => x.Brand).Include(x => x.Category)
                                    .Include(x => x.ProductPrices).ThenInclude(x => x.Unit)
                                    .Where(x => brand != null ? brand.Equals(x.Brand) : x.Brand != null
                                             && category != null ? category.Equals(x.Category) : x.Category != null)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Product>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        private IQueryable<Product> ApplyProductFiltering(IQueryable<Product> query, ProductFilter filter)
        {

            if (filter.CategoryId != null && filter.CategoryId > 0)
            {
                query = query.Where(s => s.CategoryId == filter.CategoryId);
            }

            if (filter.BrandId != null && filter.BrandId > 0)
            {
                query = query.Where(s => s.BrandId == filter.BrandId);
            }

            if (filter.StoreId != null && filter.StoreId > 0)
            {
                query = query.Where(s => s.StoreId == filter.StoreId);
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(s => s.UnsignName.Contains(filter.Search) || s.Name.Contains(filter.Search));
            }

            if (filter.MinPrice != 0 && filter.MaxPrice != 0 && filter.MinPrice < filter.MaxPrice)
            {
                query = query.Where(s => s.ProductPrices.All(x => x.Price >= filter.MinPrice && x.Price <= filter.MaxPrice));
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "name":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.UnsignName) : query.OrderBy(s => s.UnsignName);
                        break;
                    case "date":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.CreateDate) : query.OrderBy(s => s.CreateDate);
                        break;
                    case "sku":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.Sku) : query.OrderBy(s => s.Sku);
                        break;
                    case "sold":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.SoldQuantity) : query.OrderBy(s => s.SoldQuantity);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id);
                        break;
                }
            }

            return query;
        }

        public void UpdateProductAsync(Product updateProduct)
        {
            if (updateProduct.Quantity > 0)
            {
                updateProduct.IsAvailable = true;
            }
            else
            {
                updateProduct.IsAvailable = false;
            }
            updateProduct.UpdateDate = CommonUtils.GetCurrentTime();
            _dbSet.Update(updateProduct);
        }

        public void UpdateRangeProductAsync(List<Product> updateProducts)
        {
            foreach (var updateProduct in updateProducts)
            {
                if (updateProduct.Quantity > 0)
                {
                    updateProduct.IsAvailable = true;
                }
                else
                {
                    updateProduct.IsAvailable = false;
                }
                updateProduct.UpdateDate = CommonUtils.GetCurrentTime();
            }
            _dbSet.UpdateRange(updateProducts);
        }

        public async Task<Product> GetLastStoreProductAsync(int storeId)
        {
            return await _context.Products.OrderBy(x => x.Sku).LastOrDefaultAsync(x => x.StoreId == storeId);
        }
    }
}
