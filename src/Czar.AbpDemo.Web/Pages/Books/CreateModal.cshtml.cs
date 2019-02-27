using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czar.AbpDemo.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Czar.AbpDemo.BookStore;

namespace Czar.AbpDemo.Web.Pages.Books
{
    /// <summary>
    /// 继承了 BookStorePageModelBase 而非默认的 PageModel. 
    /// BookStorePageModelBase 继承了 PageModel 并且添加了一些Razor页面模型通用的属性和方法.
    /// </summary>
    public class CreateModalModel : AbpDemoPageModelBase
    {
        /// <summary>
        /// 在 Book 属性上标记的 [BindProperty] 特性绑定了post请求提交上来的数据.
        /// </summary>
        [BindProperty]
        public CreateUpdateBookDto Book { get; set; }

        private readonly IBookAppService _bookAppService;

        /// <summary>
        /// 该类通过构造函数注入了 IBookAppService 应用服务.
        /// </summary>
        /// <param name="bookAppService"></param>
        public CreateModalModel(IBookAppService bookAppService)
        {
            _bookAppService = bookAppService;
        }

        /// <summary>
        /// 并且在 OnPostAsync 方法中调用了服务的 CreateAsync 方法
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.CreateAsync(Book);
            return NoContent();
        }
    }
}