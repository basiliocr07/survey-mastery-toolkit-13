
using SurveyApp.Application.Interfaces;
using SurveyApp.Domain.Models;
using SurveyApp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyApp.Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;

        public SurveyService(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        public async Task<IEnumerable<Survey>> GetAllSurveysAsync()
        {
            return await _surveyRepository.GetAllAsync();
        }

        public async Task<Survey?> GetSurveyByIdAsync(int id)
        {
            return await _surveyRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateSurveyAsync(Survey survey)
        {
            return await _surveyRepository.AddAsync(survey);
        }

        public async Task<bool> UpdateSurveyAsync(Survey survey)
        {
            return await _surveyRepository.UpdateAsync(survey);
        }

        public async Task<bool> DeleteSurveyAsync(int id)
        {
            return await _surveyRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Survey>> GetSurveysByStatusAsync(string status)
        {
            var surveys = await _surveyRepository.GetAllAsync();
            return status.ToLower() switch
            {
                "active" => surveys.Where(s => s.Status == "active"),
                "draft" => surveys.Where(s => s.Status == "draft"),
                "archived" => surveys.Where(s => s.Status == "archived"),
                _ => surveys
            };
        }

        public async Task<bool> SendSurveyEmailsAsync(int surveyId, List<string> emailAddresses)
        {
            // In a real implementation, this would connect to an email service
            // For now, we just return true to simulate success
            var survey = await _surveyRepository.GetByIdAsync(surveyId);
            if (survey == null || emailAddresses.Count == 0)
                return false;

            // Email sending logic would go here
            return true;
        }

        // NEW: Get survey response statistics
        public async Task<SurveyStatistics> GetSurveyStatisticsAsync(int surveyId)
        {
            var survey = await _surveyRepository.GetByIdAsync(surveyId);
            if (survey == null)
                return new SurveyStatistics { SurveyId = surveyId };

            // In a real implementation, this would calculate statistics
            // from actual response data in the database
            return new SurveyStatistics
            {
                SurveyId = surveyId,
                TotalResponses = survey.ResponseCount,
                CompletionRate = survey.CompletionRate,
                AverageCompletionTime = 120, // 2 minutes (sample data)
                StartDate = survey.CreatedAt,
                EndDate = null // Ongoing survey
            };
        }
    }

    // NEW: Class to hold survey statistics
    public class SurveyStatistics
    {
        public int SurveyId { get; set; }
        public int TotalResponses { get; set; }
        public int CompletionRate { get; set; }
        public int AverageCompletionTime { get; set; } // In seconds
        public System.DateTime StartDate { get; set; }
        public System.DateTime? EndDate { get; set; }
    }
}
