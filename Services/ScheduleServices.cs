using PRN211_Project.Models;

namespace PRN211_Project.Services
{
    public class ScheduleServices
    {
        public bool CheckRequired(List<WeeklyTimeTable> schedules, int roomId, int slotId, int classId, int teacherId, int courseId)
        {
            if (schedules.Where(w => (w.RoomsId == roomId && w.TimeSlotId == slotId && w.TeachersId == teacherId &&
                                      w.ClassId == classId && w.CourseId != courseId) ||
                                     (w.RoomsId == roomId && w.TimeSlotId == slotId && w.TeachersId == teacherId &&
                                      w.ClassId != classId && w.CourseId == courseId) ||
                                     (w.RoomsId == roomId && w.TimeSlotId == slotId && w.TeachersId != teacherId &&
                                      w.ClassId == classId && w.CourseId == courseId) ||
                                     (w.RoomsId == roomId && w.TimeSlotId == slotId && w.TeachersId == teacherId &&
                                      w.ClassId == classId && w.CourseId == courseId) ||
                                     (w.RoomsId == roomId && w.TimeSlotId == slotId && w.ClassId != classId)).FirstOrDefault() != null)
                return false;

            return true;
        }
    }
}
