using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using LibrarySystem.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using LibrarySystem.DTO;
namespace LibrarySystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;

    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return Books.FromSqlRaw("EXEC sp_SearchBooks @SearchTerm",
                new SqlParameter("@SearchTerm", searchTerm)).ToList();
        }

        public string BorrowBook(int bookId, int memberId, DateTime dueDate)
        {
            var bookIdParam = new SqlParameter("@BookId", bookId);
            var memberIdParam = new SqlParameter("@MemberId", memberId);
            var dueDateParam = new SqlParameter("@DueDate", dueDate);

            var result = Database.SqlQueryRaw<string>(
                "EXEC sp_BorrowBook @BookId, @MemberId, @DueDate",
                bookIdParam, memberIdParam, dueDateParam).FirstOrDefault();

            return result ?? "Error occurred";
        }

        public async Task<string> ReturnBookAsync(int borrowingId)
        {
            var resultParam = new SqlParameter
            {
                ParameterName = "@Result",
                SqlDbType = SqlDbType.NVarChar,
                Size = -1,
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
            new SqlParameter("@BorrowingId", SqlDbType.Int) { Value = borrowingId },
            resultParam
        };

            await Database.ExecuteSqlRawAsync(
                "EXEC sp_ReturnBook @BorrowingId, @Result OUTPUT",
                parameters);

            return resultParam.Value?.ToString() ?? "Error occurred";
        }

        public IEnumerable<BorrowingRecord> GetMemberActiveBorrowings(int memberId)
        {
            return BorrowingRecords.FromSqlRaw("EXEC sp_GetMemberActiveBorrowings @MemberId",
                new SqlParameter("@MemberId", memberId)).ToList();
        }

        public IEnumerable<BorrowingRecord> GetOverdueBooks()
        {
            return BorrowingRecords.FromSqlRaw("EXEC sp_GetOverdueBooks").ToList();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowingRecords)
                .HasForeignKey(br => br.BookId);

            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(br => br.Member)
                .WithMany(m => m.BorrowingRecords)
                .HasForeignKey(br => br.MemberId);
        }
        public decimal CalculateFine(int borrowingId)
        {
            try
            {
                var parameter = new SqlParameter("@BorrowingId", SqlDbType.Int) { Value = borrowingId };
                var result = Database.SqlQueryRaw<decimal>("SELECT dbo.fn_CalculateFine(@BorrowingId)", parameter)
                    .AsEnumerable()
                    .FirstOrDefault();
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public bool CanMemberBorrow(int memberId)
        {
            return Database.SqlQueryRaw<bool>(
                "SELECT dbo.fn_CanMemberBorrow(@MemberId)",
                new SqlParameter("@MemberId", memberId))
                .FirstOrDefault();
        }

        public IQueryable<BorrowingDetails> BorrowingDetails
        {
            get { return Set<BorrowingDetails>().FromSqlRaw("SELECT * FROM VW_BorrowingDetails"); }

        }
        public async Task<string> BorrowBookAsync(int bookId, int memberId, DateTime dueDate)
        {
            var resultParam = new SqlParameter
            {
                ParameterName = "@Result",
                SqlDbType = SqlDbType.NVarChar,
                Size = -1,
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
            new SqlParameter("@BookId", SqlDbType.Int) { Value = bookId },
            new SqlParameter("@MemberId", SqlDbType.Int) { Value = memberId },
            new SqlParameter("@DueDate", SqlDbType.DateTime) { Value = dueDate },
            resultParam
        };

            try
            {
                await Database.ExecuteSqlRawAsync(
                    "EXEC sp_BorrowBook @BookId, @MemberId, @DueDate, @Result OUTPUT",
                    parameters);

                return resultParam.Value?.ToString() ?? "Error occurred";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<List<OverdueBook>> GetOverdueBooksAsync()
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXEC sp_GetOverdueBooks";
                command.CommandType = CommandType.Text;

                if (command.Connection.State != ConnectionState.Open)
                    await command.Connection.OpenAsync();

                var result = new List<OverdueBook>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new OverdueBook
                        {
                            Title = reader.GetString(0),
                            Author = reader.GetString(1),
                            MemberName = reader.GetString(2),
                            BorrowDate = reader.GetDateTime(3),
                            DueDate = reader.GetDateTime(4),
                            DaysOverdue = reader.GetInt32(5)
                        });
                    }
                }
                return result;
            }
        }

        public async Task<List<ActiveBorrowing>> GetMemberActiveBorrowingsAsync(int memberId)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXEC sp_GetMemberActiveBorrowings @MemberId";
                command.CommandType = CommandType.Text;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@MemberId";
                parameter.Value = memberId;
                command.Parameters.Add(parameter);

                if (command.Connection.State != ConnectionState.Open)
                    await command.Connection.OpenAsync();

                var result = new List<ActiveBorrowing>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ActiveBorrowing
                        {
                            Title = reader.GetString(0),
                            Author = reader.GetString(1),
                            BorrowDate = reader.GetDateTime(2),
                            DueDate = reader.GetDateTime(3)
                        });
                    }
                }
                return result;
            }
        }

    }
}
