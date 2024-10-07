﻿using AutoMapper;
using Fricks.Repository.Commons;
using Fricks.Repository.Entities;
using Fricks.Repository.UnitOfWork;
using Fricks.Service.BusinessModel.CategoryModels;
using Fricks.Service.BusinessModel.FavoriteProductModels;
using Fricks.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fricks.Service.Services
{
    public class FavoriteProductService : IFavoriteProductService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public FavoriteProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FavoriteProductModel> AddFavoriteProduct(string email, FavoriteProductProcessModel favoriteProduct)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }

            var existProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(favoriteProduct.ProductId);
            if (existProduct == null)
            {
                throw new Exception("Sản phẩm không tồn tại");
            }

            var newFavProduct = new FavoriteProduct
            {
                ProductId = favoriteProduct.ProductId,
                UserId = user.Id
            };

            var result = await _unitOfWork.FavoriteProductRepository.AddAsync(newFavProduct);

            _unitOfWork.Save();
            return _mapper.Map<FavoriteProductModel>(result);
        }

        public async Task<FavoriteProductModel> DeleteFavoriteProduct(int id)
        {
            var favProduct = await _unitOfWork.FavoriteProductRepository.GetByIdAsync(id);
            if (favProduct == null)
            {
                throw new Exception("Không tìm thấy sản phẩm yêu thích - Không thể xóa");
            }
            _unitOfWork.FavoriteProductRepository.PermanentDeletedAsync(favProduct);
            _unitOfWork.Save();
            return _mapper.Map<FavoriteProductModel>(favProduct);
        }

        public async Task<List<FavoriteProductModel>> GetAllFavoriteProduct()
        {
            var listFavProduct = await _unitOfWork.FavoriteProductRepository.GetAllAsync();
            return _mapper.Map<List<FavoriteProductModel>>(listFavProduct);
        }

        public async Task<Pagination<FavoriteProductModel>> GetUserFavoriteProductsPagination(string email, PaginationParameter paginationParameter)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }
            var listFavProduct = await _unitOfWork.FavoriteProductRepository.GetFavoriteProductPaging(user.Id, paginationParameter);
            return _mapper.Map<Pagination<FavoriteProductModel>>(listFavProduct);
        }
    }
}
