﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Raven.Tests.Document {
  public class PerCollectionEtag : RavenTest {
     
    [Fact]
    public void CanRetreivePerCollectionETag()
    {
      using (var store = NewDocumentStore(requestedStorage: "esent"))
      {
        using (var session = store.OpenSession())
        {
          session.Store(new Post { Id = "posts/1", Title = "test", Body = "etags"});
          session.Store(new Comment { Id = "comments/1" ,Title = "test"});
          session.SaveChanges();
        }

        var postsCollectioEtag = store.DocumentDatabase.GetLastEtagForCollection("Posts");
        var commentsCollectionEtag = store.DocumentDatabase.GetLastEtagForCollection("Comments");

        var postEtag = store.DatabaseCommands.Head("posts/1").Etag;
        var commentETag = store.DatabaseCommands.Head("comments/1").Etag;

        Assert.Equal(postEtag, postsCollectioEtag);
        Assert.Equal(commentETag, commentsCollectionEtag);
      }


    }

    public class Post
    {
      public string Id { get; set; }
      public string Title { get; set; }
      public string Body { get; set; }
    }

    public class Comment
    {
      public string Id { get; set; }
      public string Title { get; set; }
    }
  }
}
