using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("name=SchoolDBConnectionString")
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here

            base.OnModelCreating(modelBuilder);
        }
    }
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Url]
        public string Url { get; set; }

        public List<Post> Posts { get; set; }

        public string Tags { get; set; }

        [NotMapped]
        public string[] _Tags
        {
            get { return Tags == null ? null : JsonConvert.DeserializeObject<string[]>(Tags); }
            set { Tags = JsonConvert.SerializeObject(value); }
        }

        public string Owner { get; set; }

        [NotMapped]
        public Person _Owner
        {
            get { return (this.Owner == null) ? null : JsonConvert.DeserializeObject<Person>(this.Owner); }
            set { Owner = JsonConvert.SerializeObject(value); }
        }
    }

    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }

    public class Person
    {
        [Required]
        public string Name { get; set; }

        public string Surname { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}