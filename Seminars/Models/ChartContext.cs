using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Seminars.Models;

public class ChartContext: DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        string filename = "Application.db";
        string directory = @"C:\\Users\\shado\\OneDrive\\Desktop\\GB\\Разработка сетевого приложения С#\\Урок 5. Базы данных Entity framework, code firstdb first\\Seminar\\Seminars\\Seminars";
        string filepath = Path.Combine(directory, filename);
       StringBuilder sb = new StringBuilder();

        sb.Append("Data Source=");
        sb.Append(filepath);
        
        const string baseConnectionString =
            "Data Source=Application.db";
        string connectionString = new SqliteConnectionStringBuilder(sb.ToString())
        {
            Mode = SqliteOpenMode.ReadWriteCreate,
        }.ToString();
        //"Data Source=192.168.50.40,1433;Initial Catalog=myDataBase;User ID=admin;Password=StrongP@ssword;Encrypt=True;TrustServerCertificate=True;"
        //Data Source=192.168.50.40,1433;Network Library=DBMSSOCN;Initial Catalog=myDataBase;User ID=myUsername;Password=mssql1Ipw;
        optionsBuilder
            .UseSqlite(connectionString)
            .UseLazyLoadingProxies();
    }
    //https://www.connectionstrings.com/mysql/
    //https://citizix.com/how-to-run-mssql-server-2019-with-docker-and-docker-compose/

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // modelBuilder.Entity<User>().HasKey(x => x.UserId).HasName("user_pk");
        modelBuilder.Entity<User>(user =>
        {
            user
                .ToTable("users");
            
            user
                .HasKey(x => x.UserId)
                .HasName("user_pk");
            
            user
                .Property(Users =>
                    Users.FullName
                )
                .HasColumnName("full_name")
                .HasMaxLength(255)
                .IsRequired();
            
            user
                .HasIndex(x => x.FullName)
                .IsUnique();
        });


        modelBuilder.Entity<Message>(
            message =>
            {
                message
                    .ToTable("messages");
                
                message
                    .HasKey(x => x.MessageId)
                    .HasName("message_pk");
                
                message
                    .Property(Messages =>
                        Messages.Text
                    )
                    .HasColumnName("text")
                    .HasMaxLength(255);
                
                message
                    .Property(e => e.SendDate)
                    .HasColumnName("send_date");
                
                message
                    .Property(e => e.IsSent)
                    .HasColumnName("is_sent");

                message
                    .HasOne(x => x.UserFrom)
                    .WithMany(x => x.MessagesFrom)
                    .HasConstraintName("message_from_fk")
                    .HasForeignKey(x=>x.UserFromId)
                    .HasConstraintName("massage_from_user_fk");

                message
                    .HasOne(x => x.UserTo)
                    .WithMany(x => x.MessagesTo)
                    .HasConstraintName("message_to_fk")
                    .HasForeignKey(x=>x.UserToId)
                    .HasConstraintName("massage_to_user_fk");
                
                ///dotnet ef migrations add InitialCreate
            });
    }

    public ChartContext() { }
}