using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using System.Threading.Tasks;
using Boo.Blog.Response;
using Boo.Blog.Domain.Blog.IRepositories;
using System.Linq;
using Volo.Abp.Uow;

namespace Boo.Blog.Application.Blog
{
    public class BlogService : ServiceBase<Post, PostDto, long>, IBlogService
    {
        readonly IPostRepository _postRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IFriendLinkRepository _friendLinkRepository;
        readonly ITagRepository _tagRepository;
        readonly IPostTagRepository _postTagRepository;
        readonly IUnitOfWorkManager _unitOfWork;
        public BlogService(IPostRepository postRepository,
            ICategoryRepository categoryRepository,
            IFriendLinkRepository friendLinkRepository,
            ITagRepository tagRepository,
            IPostTagRepository postTagRepository,
            IUnitOfWorkManager unitOfWork) : base(postRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _friendLinkRepository = friendLinkRepository;
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;

            _unitOfWork = unitOfWork;
        }
        

        /// <summary>
        /// 获取博客完整信息
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        public async Task<ResponseDataResult<PostFullInfoDto>> GetPostFullInfoAsync(long id)
        {

            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                return ResponseResult.IsFail<PostFullInfoDto>("数据不存在");
            }
            var postFullInfo = ObjectMapper.Map<Post, PostFullInfoDto>(post);
            if (post.CategoryId != 0)
            {
                postFullInfo.CategoryInfo = await _categoryRepository.GetAsync(post.CategoryId);
            }
            var tagList = await _postTagRepository.GetListAsync(a => a.PostId == post.Id);
            postFullInfo.Tags = await _tagRepository.GetListAsync(a => tagList.Select(t => t.Id).Contains(a.Id));
            postFullInfo.FriendLinks = await _friendLinkRepository.GetListAsync();
            return ResponseResult.IsSuccess(postFullInfo);
        }

        public async Task<ResponseDataResult<long>> GetPostsCountAsync()
        {
            var data = await _postRepository.GetCountAsync();
            return ResponseResult.IsSuccess(data, "ok");
        }

        public async Task DeletePostAsync(int id)
        { 
            await  _postRepository.DeleteAsync(id);
        }

        public async Task TestUowAsync()
        {
            var uow = _unitOfWork.Begin(new AbpUnitOfWorkOptions());
            await _postRepository.InsertAsync(new Post { Title="test uow" });
            await _postTagRepository.DeleteAsync(a=>a.Id==2);
            throw new System.Exception("test uow error");
           await uow.CompleteAsync();
        }
    }
}
