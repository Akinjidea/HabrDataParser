using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    internal class Article
    {
        public object id;
        public object nameAuthor;
        public object dateArticle;
        public object nameArticle;
        public object rating;
        public object bookmarks;
        public object views;
        public object code;

        public List<Comments> comments;
    } 
}
