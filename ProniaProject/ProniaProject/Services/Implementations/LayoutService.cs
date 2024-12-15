using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Services.Interfaces;
using ProniaProject.ViewModels;
using System.Security.Claims;

namespace ProniaProject.Services.Implementations
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
       
        

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Dictionary<string,string>> GetSettingAsync()
        {
            Dictionary<string,string> settings = await _context.Settings.ToDictionaryAsync(s=>s.Key, s=>s.Value);

            return settings;
        }
    }
}
