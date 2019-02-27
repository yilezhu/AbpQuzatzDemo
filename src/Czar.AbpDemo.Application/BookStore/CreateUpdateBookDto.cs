/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：创建和更新书籍的时候被使用,用来从页面获取图书信息                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/19 14:24:33                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.BookStore                                   
*│　类    名： CreateUpdateBookDto                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Czar.AbpDemo.BookStore
{
    [AutoMapTo(typeof(Book))]
    [AutoMapFrom(typeof(BookDto))]
    public class CreateUpdateBookDto
    {
        [Required, StringLength(128)]
        public string Name { get; set; }
        [Required]
        public BookType Type { get; set; } = BookType.Undefined;
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
