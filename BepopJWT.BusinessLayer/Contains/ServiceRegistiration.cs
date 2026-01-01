using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Concrete;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Contains
{
    public static class ServiceRegistiration
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthManager>();

            services.AddScoped<IArtistDal, EfArtistDal>();
            services.AddScoped<IArtistService, ArtistManager>();

            services.AddScoped<ICategoryDal, EfCategoryDal>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IPackageDal, EfPackageDal>();
            services.AddScoped<IPackageService, PackageManager>();

            services.AddScoped<IPaymentDal, EfPaymentDal>();
            services.AddScoped<IPaymentService, PaymentManager>();

            services.AddScoped<IIyzicoService, IyzicoManager>();

            services.AddScoped<IOrderDal, EfOrderDal>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IUserService, UsersManager>();

            services.AddScoped<ITokenService, TokenManager>();

            services.AddScoped<ISongDal, EfSongDal>();
            services.AddScoped<ISongService, SongManager>();

            services.AddScoped<IFileUploadService, FileUploadManager>();

            services.AddScoped<IPlaylistDal, EfPlaylistDal>();
            services.AddScoped<IPlaylistSongDal, EfPlaylistSongDal>();
            services.AddScoped<IPlayListService, PlaylistManager>();

            services.AddScoped<IMLRecommendationService, MLRecommendationManager>();

            services.AddHttpClient<IOpenAIService, OpenAIManager>();
        }
    }
}
