/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：图书测试类                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/20 18:22:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo                                   
*│　类    名： BookAppService_Tests                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Czar.AbpDemo.BookStore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Xunit;

namespace Czar.AbpDemo
{
    public class BookAppService_Tests:AbpDemoApplicationTestBase
    {
        private readonly IBookAppService _bookAppService;

        public BookAppService_Tests()
        {
            _bookAppService = GetRequiredService<IBookAppService>(); ;
        }

        [Fact]
        public async Task Should_Get_List_Of_Books()
        {
            //Act
            var result = await _bookAppService.GetListAsync(
                new PagedAndSortedResultRequestDto()
            );

            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(b => b.Name == "Test book 1");
            result.Items.ShouldContain(b => b.Name == "Test book 2");
        }

        [Fact]
        public async Task Should_Create_A_Valid_Book()
        {
            var result = await _bookAppService.CreateAsync(
                new CreateUpdateBookDto {
                    Name = "New test book 3",
                    Price = 10.3F,
                    PublishDate = DateTime.Now,
                    Type = BookType.ScienceFiction
                });

            //Assert
            result.Id.ShouldBeGreaterThan(0);
            result.Name.ShouldBe("New test book 3");
        }

        [Fact]
        public async Task Should_Not_Create_A_Book_Without_Name()
        {
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _bookAppService.CreateAsync(
                    new CreateUpdateBookDto
                    {
                        Name = "",
                        Price = 10,
                        PublishDate = DateTime.Now,
                        Type = BookType.ScienceFiction
                    }
                );
            });

            exception.ValidationErrors
                .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
        }
    }
}
