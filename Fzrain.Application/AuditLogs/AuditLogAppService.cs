﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Auditing;
using Fzrain.AuditLogs.Dto;

namespace Fzrain.AuditLogs
{
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IRepository<AuditLog,long> auditRepository;

        public AuditLogAppService(IRepository<AuditLog, long> auditRepository)
        {
            this.auditRepository = auditRepository;
        }
        [AbpAuthorize("Administration.AuditLog.Read")]
        public PagedResultOutput<AuditLogDto> GetAuditLogs(GetAuditLogInput input)
        {
            var auditLogCount = auditRepository.Count();
            var auditLogs = auditRepository.GetAll().OrderByDescending(a=>a.ExecutionTime).PageBy(input).ToList();
            return new PagedResultOutput<AuditLogDto>
            {
                Items = auditLogs.MapTo<List<AuditLogDto>>(),
                TotalCount = auditLogCount,
            };
        }
        [AbpAuthorize("Administration.AuditLog.Detail")]
        public async Task<AuditLogDto> GetDetail(IdInput<long> input)
        {
            var auditLog= await auditRepository.GetAsync(input.Id);
            return auditLog.MapTo<AuditLogDto>();
        }
    }
}
