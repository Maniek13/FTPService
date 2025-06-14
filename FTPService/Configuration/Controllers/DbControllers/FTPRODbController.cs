﻿using Configuration.Data;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;

namespace Configuration.Controllers.DbControllers
{
    public class FTPRODbController() : IFTPRODbController
    {
        public ServicesPermisionsDbModel GetPermision(string serviceName)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return _context.ServicesPermisions.Where(el => el.ServiceName == serviceName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public FTPConfigurationDbModel GetFTPConfiguration(int serviceId)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return _context.FtpConfigurations.Where(el => el.ServiceId == serviceId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<ServiceActionDbModel> GetServiceActions(int serviceId)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return [.. _context.FtpServicesActions.Where(el => el.ServiceId == serviceId)];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ServiceActionDbModel GetServiceAction(int serviceId, string actionName)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return _context.FtpServicesActions.Where(el => el.ServiceId == serviceId && el.ActionName == actionName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public ServiceActionDbModel GetServiceAction(int actionId)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return _context.FtpServicesActions.Where(el => el.Id == actionId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public FileDbModel GetFile(int id)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return _context.FtpFiles.Where(el => el.Id == id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public FileDbModel GetFile(int serviceActionId, string fileName)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return _context.FtpFiles.Where(el => el.ServiceActionId == serviceActionId && el.Name == fileName).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public List<FileDbModel> GetActionFiles(int actionId)
        {
            try
            {
                using FTPServiceContextRO _context = new(AppConfig.ConnectionStringRO);
                return [.. _context.FtpFiles.Where(el => el.ServiceActionId == actionId)];

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
