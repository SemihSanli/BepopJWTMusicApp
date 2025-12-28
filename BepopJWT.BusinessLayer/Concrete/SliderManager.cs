using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class SliderManager : ISliderService
    {
        private readonly ISliderDal _sliderDal;

        public SliderManager(ISliderDal sliderDal)
        {
            _sliderDal = sliderDal;
        }

        public async Task TAddAsync(Slider entity)
        {
            await _sliderDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _sliderDal.DeleteAsync(id);
        }

        public async Task<List<Slider>> TGetAllAsync()
        {
           return await _sliderDal.GetAllAsync();
        }

        public async Task<Slider> TGetByIdAsync(int id)
        {
            return await _sliderDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Slider entity)
        {
            await _sliderDal.UpdateAsync(entity);
        }
    }
}
