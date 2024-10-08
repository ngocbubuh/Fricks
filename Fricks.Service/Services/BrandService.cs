﻿using AutoMapper;
using Fricks.Repository.Commons;
using Fricks.Repository.Entities;
using Fricks.Repository.UnitOfWork;
using Fricks.Service.BusinessModel.BrandModels;
using Fricks.Service.BusinessModel.ProductModels;
using Fricks.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fricks.Service.Services
{
    public class BrandService : IBrandService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BrandModel> AddBrand(BrandProcessModel brand)
        {
            // check duplicate
            var existBrand = await _unitOfWork.BrandRepository.GetAllAsync();
            var checkDuplicate = existBrand.FirstOrDefault(x => x.Name.ToLower() == brand.Name.ToLower());
            if (checkDuplicate != null)
            {
                throw new Exception("Hãng đã tồn tại");
            }

            var addBrand = _mapper.Map<Brand>(brand);
            var result = await _unitOfWork.BrandRepository.AddAsync(addBrand);
            _unitOfWork.Save();
            return _mapper.Map<BrandModel>(result);
        }

        public async Task<BrandModel> DeleteBrand(int id)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                throw new Exception("Không tìm thấy hãng - Không thể xóa");
            }

            // check product
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            bool checkUsingProduct = products.Any(product => product.BrandId == brand.Id);
            if (checkUsingProduct)
            {
                throw new Exception("Không thể xóa hãng do có sản phẩm đang dùng");
            }

            _unitOfWork.BrandRepository.SoftDeleteAsync(brand);
            _unitOfWork.Save();
            return _mapper.Map<BrandModel>(brand);
        }

        public async Task<List<BrandModel>> GetAllBrand()
        {
            var listBrand = await _unitOfWork.BrandRepository.GetAllAsync();
            return _mapper.Map<List<BrandModel>>(listBrand);
        }

        public async Task<Pagination<BrandModel>> GetAllBrandPagination(PaginationParameter paginationParameter)
        {
            var listBrand = await _unitOfWork.BrandRepository.GetBrandPaging(paginationParameter);
            return _mapper.Map<Pagination<BrandModel>>(listBrand);
        }

        public async Task<BrandModel> GetBrandById(int id)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
            return _mapper.Map<BrandModel>(brand);
        }

        public async Task<BrandModel> UpdateBrand(int id, BrandProcessModel brandModel)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                throw new Exception("Không tìm thấy hãng - Không thể cập nhật");
            }

            // check duplicate
            var existBrand = await _unitOfWork.BrandRepository.GetAllAsync();
            var checkDuplicate = existBrand.FirstOrDefault(x => x.Name.ToLower() == brandModel.Name.ToLower());
            if (checkDuplicate != null)
            {
                throw new Exception("Hãng đã tồn tại");
            }

            var updateBrand = _mapper.Map(brandModel, brand);
            _unitOfWork.BrandRepository.UpdateAsync(updateBrand);
            _unitOfWork.Save();
            return _mapper.Map<BrandModel>(updateBrand);
        }
    }
}
