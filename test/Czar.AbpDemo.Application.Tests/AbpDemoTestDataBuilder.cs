using System;
using System.Threading.Tasks;
using Czar.AbpDemo.BookStore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Czar.AbpDemo
{
    public class AbpDemoTestDataBuilder : ITransientDependency
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;
        private readonly IRepository<BookStore.Book, Int32> _bookRepository;

        public AbpDemoTestDataBuilder(IIdentityDataSeeder identityDataSeeder, 
            IRepository<Book, int> bookRepository)
        {
            _identityDataSeeder = identityDataSeeder;
            _bookRepository = bookRepository;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildInternalAsync);
        }

        /// <summary>
        /// 直接使用了identity模块实现的 IIdentityDataSeeder 接口,创建了一个admin角色和admin用户
        /// </summary>
        /// <returns></returns>
        public async Task BuildInternalAsync()
        {
            await _identityDataSeeder.SeedAsync("1q2w3E*");
            await _bookRepository.InsertAsync(
                new Book
                {
                    Name = "Test book 1",
                    Type = BookType.Fantastic,
                    PublishDate = new DateTime(2019, 02, 20),
                    Price = 21.5F
                }
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    Name = "Test book 2",
                    Type = BookType.Science,
                    PublishDate = new DateTime(2018, 02, 11),
                    Price = 15.4F
                }
            );
        }
    }
}