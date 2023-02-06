using AutoMapper;
using E_commerce_Website.Data;
using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using E_commerce_Website.Extensitions;
using E_commerce_Website.RequestHelpers;
using E_commerce_Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace E_commerce_Website.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public ProductsController(StoreContext context , IMapper mapper , ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }
        /// <remarks>
        /// The API takes the string authorization, languageId, search, isDeleted,  isLocked,  isActive,  IsDefaultPolicy,  entities,  entityTypes,  roles,  specialPasswordComplexity,  branchId,  page and  pageSize from header.
        /// 
        /// API Headers
        /// ===========
        /// authorization* : The access token received when the login was executed
        /// languageId*:  the language Id of the current User
        /// search:  the search string 
        /// isDeleted: boolean 
        /// isLocked:  boolean
        /// isActive:  boolean
        /// IsDefaultPolicy:  boolean
        /// entities:  string
        /// entityTypes:  string
        /// roles:  string roles 
        /// specialPasswordComplexity*:  boolean
        /// branchId*:  Int64
        /// sort:  string (example: "F" for Fullname,"U" for username and "E" for email )
        /// page*:  int
        /// pageSize*:int
        /// API Input
        /// =========
        /// API URL
        /// No API URL input is required to call this API
        /// ===========
        /// No body input is required to call this API
        /// </remarks>
        //[HttpGet(Name ="Get Products")]
        //public async Task<ActionResult<PagedList<Product>>> GetProduct([FromQuery]ProductParms productParms)
        //{
        //    var query = _context.Products
        //        .sort(productParms.OrderBy)
        //        .Search(productParms.SearchTerm)
        //        .Filter(productParms.Brand,productParms.Type)
        //        .AsQueryable();
             
        //    var products = await PagedList<Product>.ToPageList(query,productParms.PageNumber,productParms.PageSize);

        //   /* var fileName = "xyz.json";

        //    var json_data = string.Empty;
        //    using (var w = new WebClient())
        //    {
        //        // attempt to download JSON data as a string
        //        json_data = w.DownloadString("http://localhost:7539/swagger/v1/swagger.json");
        //    }
        //    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json_data);
        //    var content = new MemoryStream(bytes);
        //    return File(content, "application/json", fileName);*/

        //    Response.AddPaginationHeader(products.MetaData);

        //    return products;
        //}
        /// <remarks> ****GET**** /api/Products/id</remarks>
        //[HttpGet("{id}", Name = "Get Product")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null) return NotFound();
        //    return product;
        //}
        /// <remarks> ****GET**** /api/Products/filters</remarks>
        //[HttpGet("filters",Name = "Get Filters")]
        //public async Task<IActionResult> GetFilters()
        //{
        //    var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        //    var type = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

        //    return Ok(new { brands, type });
        //}
        /// <remarks> ****POST**** /api/Products</remarks>
        //[Authorize(Roles ="Admin")]
        //[HttpPost(Name = "Create Product")]
        //public async Task<ActionResult<Product>> CreateProduct([FromForm]CreateProductDTO productDTO)
        //{
        //    var product = _mapper.Map<Product>(productDTO);
        //    if(productDTO.File != null)
        //    {
        //        var imageResult = await _imageService.AddImageAsync(productDTO.File);
        //        if (imageResult.Error != null)
        //            return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });
        //        product.PictureUrl = imageResult.SecureUrl.ToString();
        //        product.PublicId = imageResult.PublicId;
        //    }
        //    _context.Products.Add(product);
        //    var result = await _context.SaveChangesAsync() > 0;
        //    if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);
        //    return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        //}
        /// <remarks> ****PUT**** /api/Products</remarks>
        //[Authorize(Roles = "Admin")]
        //[HttpPut(Name ="Update Product")]
        //public async Task<ActionResult> UpdateProduct([FromForm]UpdateProductDTO updateProdct)
        //{
        //    var product = await _context.Products.FindAsync(updateProdct.Id);
        //    if (product == null) return NotFound();
        //    _mapper.Map(updateProdct, product);
        //    if(updateProdct.File != null)
        //    {
        //        var imageResult = await _imageService.AddImageAsync(updateProdct.File);
        //        if (imageResult.Error != null)
        //            return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });
        //        if (!string.IsNullOrEmpty(product.PublicId))
        //            await _imageService.DeleteImage(product.PublicId);
        //        product.PictureUrl = imageResult.SecureUrl.ToString();
        //        product.PublicId = imageResult.PublicId;
        //    }
        //    var result = await _context.SaveChangesAsync() > 0;
        //    if (result) return Ok(product);
        //    return BadRequest(new ProblemDetails { Title = "Problem Updating" });
        //}
        /// <remarks> ****DELETE**** /api/Products</remarks>
        //[Authorize(Roles = "Admin")]
        //[HttpDelete(Name ="Delete Product")]
        //public async Task<ActionResult> DeleteProduct(int Id)
        //{
        //    var product = await _context.Products.FindAsync(Id);
        //    if (product == null) return NotFound();
        //    if (!string.IsNullOrEmpty(product.PublicId))
        //        await _imageService.DeleteImage(product.PublicId);
        //    _context.Products.Remove(product);
        //    var result = await _context.SaveChangesAsync() > 0;
        //    if (result) return NoContent();
        //    return BadRequest(new ProblemDetails { Title = "Problem Updating" });
        //}

    }
}
