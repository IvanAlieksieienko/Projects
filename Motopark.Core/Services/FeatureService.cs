using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motopark.Core.Services
{
    public class FeatureService : IFeatureService<Feature>
    {
        private IFeatureRepository<Feature> _featureRepository;

        public FeatureService(IFeatureRepository<Feature> featureRepository)
        {
            _featureRepository = featureRepository;
        }

        public async Task Add(Feature feature)
        {
            await _featureRepository.Add(feature);
        }

        public async Task AddFeatures(List<Feature> features)
        {
            foreach(var feature in features)
            {
                await _featureRepository.Add(feature);
            }
        }

        public async Task Delete(Guid id)
        {
            await _featureRepository.Delete(id);
        }

        public async Task DeleteByProductID(Guid id)
        {
            await _featureRepository.DeleteByProductID(id);
        }

        public async Task<Feature> GetByID(Guid id)
        {
            var feature = await _featureRepository.GetByID(id);
            return feature;
        }

        public async Task<ICollection<Feature>> GetByProductID(Guid id)
        {
            var features = await _featureRepository.GetByProductID(id);
            return features;
        }

        public async Task<Feature> Update(Feature feature)
        {
            var newFeature = await _featureRepository.Update(feature);
            return newFeature;
        }

        public async Task UpdateFeatures(List<Feature> features)
        {
            foreach(var feature in features)
            {
                await _featureRepository.Update(feature);
            }
        }
    }
}
