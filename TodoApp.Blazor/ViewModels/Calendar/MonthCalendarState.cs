namespace TodoApp.Blazor.ViewModels.Calendar
{
    /// <summary>
    /// UI state for Month Calendar (pure client-side).
    /// </summary>
    public sealed class MonthCalendarState
    {
        public DateOnly CurrentMonth { get; private set; }

        public DateOnly GridStartDate { get; private set; }
        public DateOnly GridEndDate { get; private set; }

        public MonthCalendarState()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            CurrentMonth = new DateOnly(today.Year, today.Month, 1);
            RecalculateGrid();
        }

        public void GoToPreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            RecalculateGrid();
        }

        public void GoToNextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            RecalculateGrid();
        }

        private void RecalculateGrid()
        {
            GridStartDate = GetGridStart(CurrentMonth);
            GridEndDate = GetGridEnd(CurrentMonth);
        }

        // ===============================
        // Helpers
        // ===============================

        private static DateOnly GetGridStart(DateOnly month)
        {
            // First day of month
            var firstDay = new DateOnly(month.Year, month.Month, 1);

            // Back to Sunday
            var diff = (int)firstDay.DayOfWeek;
            return firstDay.AddDays(-diff);
        }

        private static DateOnly GetGridEnd(DateOnly month)
        {
            var firstDayNextMonth = new DateOnly(month.Year, month.Month, 1)
                .AddMonths(1);

            var lastDay = firstDayNextMonth.AddDays(-1);

            // Forward to Saturday
            var diff = 6 - (int)lastDay.DayOfWeek;
            return lastDay.AddDays(diff);
        }
    }
}
