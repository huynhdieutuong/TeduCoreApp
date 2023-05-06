using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using Xunit;

namespace TeduCoreApp.Data.EF.Test
{
    public class EFUnitOfWorkTest
    {
        public EFUnitOfWorkTest()
        {
            _context = ContextFactory.Create();
        }

        private readonly AppDbContext _context;

        [Fact]
        public void Commit_Should_Success_When_Save_Data()
        {
            EFRepository<Function, string> EFRepository = new EFRepository<Function, string>(_context);
            EFUnitOfWork unitOfWork = new EFUnitOfWork(_context);
            EFRepository.Add(new Function()
            {
                Id = "USER",
                Name = "Test",
                Status = Status.Active,
                SortOrder = 1
            });
            unitOfWork.Commit();

            List<Function> functions = EFRepository.FindAll().ToList();
            Assert.Single(functions);
        }
    }
}