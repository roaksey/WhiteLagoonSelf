using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public void CreateVilla(Villa villa)
        {
            if (villa.Image is not null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");
                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
                };
            }
            else
            {
                villa.ImageUrl = "https://placehold.co/600x600/png";
            }
            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.Villa.Get(x => x.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Villa.Remove(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Villa GetVillaById(int id)
        {
            return _unitOfWork.Villa.Get(x => x.Id == id);
        }

        public IEnumerable<Villa> GetVillas()
        {
            return _unitOfWork.Villa.GetAll();
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");
                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (FileStream fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
                };
            }

            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
        }
    }
}
