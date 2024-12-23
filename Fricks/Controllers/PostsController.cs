﻿using Fricks.Repository.Commons;
using Fricks.Repository.Commons.Filters;
using Fricks.Service.BusinessModel.PostModels;
using Fricks.Service.BusinessModel.UserModels;
using Fricks.Service.Services;
using Fricks.Service.Services.Interface;
using Fricks.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fricks.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IClaimsService _claimsService;

        public PostsController(IPostService postService, IClaimsService claimsService)
        {
            _postService = postService;
            _claimsService = claimsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostAsync([FromQuery] PaginationParameter paginationParameter, [FromQuery] PostFilter postFilter)
        {
            try
            {
                var currentEmail = _claimsService.GetCurrentUserEmail;
                var result = await _postService.GetPostPaginationAsync(paginationParameter, postFilter, currentEmail);
                if (result == null)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Không có bài viết nào"
                    });
                }

                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseModel<string>
                    {
                        HttpCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message.ToString()
                    }
               );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var data = await _postService.GetPostByIdAsync(id);
                if (data == null)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        HttpCode = 404,
                        Message = "Bài viết không tồn tại"
                    });
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel<string>
                {
                    HttpCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,STORE")]
        public async Task<IActionResult> DeletePostById(int id)
        {
            try
            {
                var result = await _postService.DeletePostAsync(id);
                if (result != null)
                {
                    return Ok(new ResponseModel<string>
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = $"Xóa bài viết '{result.Title}' thành công"
                    });
                }
                return BadRequest(new ResponseModel<string>
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = "Có lỗi trong quá trình xóa bài viết"
                });
            }
            catch (Exception ex)
            {
                var resp = new ResponseModel<string>
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,STORE")]
        public async Task<IActionResult> CreatePostAsync(CreatePostModel createPostModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _postService.AddPostAsync(createPostModel);
                    var resp = new ResponseModel<string>
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Tạo bài viết thành công"
                    };
                    return Ok(resp);
                }
                return ValidationProblem(ModelState);

            }
            catch (Exception ex)
            {
                var resp = new ResponseModel<string>
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(resp);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePostAsync(UpdatePostModel postModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var updatePost = await _postService.UpdatePostAsync(postModel);

                    if (updatePost != null)
                    {
                        return Ok(new ResponseModel<string>
                        {
                            HttpCode = StatusCodes.Status200OK,
                            Message = $"Cập nhật thông tin bài viết thành công."
                        });
                    }
                    return NotFound(new ResponseModel<string>
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Có lỗi trong quá trình cập nhật thông tin bài viết."
                    });

                }
                return ValidationProblem(ModelState);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
