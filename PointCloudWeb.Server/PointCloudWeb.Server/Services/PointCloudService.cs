using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Utils;

namespace PointCloudWeb.Server.Services
{
    public class PointCloudService
    {
        //private readonly IPointCloudRegistrationService pointCloudRegistration;
        private readonly PointCloudCollection _pointClouds;

        public PointCloudService( /*IPointCloudRegistrationService pointCloudRegistration*/)
        {
            _pointClouds = new PointCloudCollection();
            //this.pointCloudRegistration = pointCloudRegistration;
            InitSampleData();
        }

        private void GeneratePotreeData(Guid id)
        {
            var pathTarget = Globals.PotreeDataPath;
            var converter = Globals.PotreeConverterExe;

            var tempFile = Globals.TempPath + $"/{id}.las";

            Directory.CreateDirectory(Globals.TempPath);

            var pc = _pointClouds.GetById(id);

            pc.WriteToLas(tempFile);
            
            
            var potreeConverter = new Process();
            potreeConverter.StartInfo.FileName = Globals.PotreeConverterExe;
            potreeConverter.StartInfo.Arguments = $"\"{tempFile}\" -o \"{Globals.TempPath}/{id.ToString()}\"";
            potreeConverter.Start();
            potreeConverter.WaitForExit();
        }

        private void InitSampleData()
        {
            EthTestData.CreateData(this);
        }

        private void RaiseIfNotExists(Guid id)
        {
            if (!_pointClouds.Contains(id))
                throw new ArgumentOutOfRangeException($"The Id {id.ToString()} was not found!");
        }

        public void AddPoints(Guid id, IList<Point> points)
        {
            RaiseIfNotExists(id);

            var pc = _pointClouds.GetById(id);

            foreach (var point in points)
                pc.Points.Add(point);
        }

        public PointCloud AddPointCloud()
        {
            var pc = new PointCloud(Guid.NewGuid(), "");
            _pointClouds.Add(pc);
            return pc;
        }

        public IEnumerable<PointCloud> GetAll() => _pointClouds;

        public PointCloud GetById(Guid id) =>  _pointClouds.GetById(id);

        public void RegisterPointCloud(Guid id)
        {
            RaiseIfNotExists(id);
            // var pointCloud = _pointClouds.GetById(id);
            //
            // //the first can't be registered
            // if (_pointClouds.IndexOf(pointCloud) == 0)
            //     return;

            //var transformation = pointCloudRegistration.RegisterPointCloud(pointCloud, pointClouds[0]);
            //pointCloud.Transformation = transformation;
        }

        public void RegisterPointClouds()
        {
            foreach (var pointCloud in _pointClouds)
                RegisterPointCloud(pointCloud.Id);
        }

        public void RemoveById(Guid id)
        {
            _pointClouds.RemoveById(id);
        }

        public void PointCloudCompleted(Guid id)
        {
            RegisterPointCloud(id);
            GeneratePotreeData(id);
        }
    }
}