﻿using Boo.Blog.MongoDB.MongoDb;
using MongoDB.Driver;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace AbotSpider.Crawlers
{
    public class CrawlerMongoDbContext : BaseMongoDbContext
    {
        public IMongoCollection<Poem> Poems => Collection<Poem>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poem>(b=>
            {
                b.CollectionName = "Poems";

            });
        }
    }

    public class MongoDbPoemRepository : MongoDbRepository<CrawlerMongoDbContext, Poem, Guid>, IRepository<Poem, Guid>, ITransientDependency
    {
        public MongoDbPoemRepository(IMongoDbContextProvider<CrawlerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }

    public class Poem : Entity<Guid>
    {
        public Poem() { }
        public Poem(Guid id):base(id) { }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        /// <summary>
        /// 朝代
        /// </summary>
        public string Dynasty { get; set; }
        public string[] Tags { get; set; }
    }
}
