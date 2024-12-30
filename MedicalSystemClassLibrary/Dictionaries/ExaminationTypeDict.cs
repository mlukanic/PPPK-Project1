using MedicalSystemClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystemClassLibrary.Dictionaries
{
    public static class ExaminationTypeDict
    {
        public static readonly Dictionary<ExaminationType, string> Descriptions = new Dictionary<ExaminationType, string>
        {
            { ExaminationType.GP, "Opći tjelesni pregled" },
            { ExaminationType.KRV, "Test krvi" },
            { ExaminationType.X_RAY, "Rendgensko skeniranje" },
            { ExaminationType.CT, "CT sken" },
            { ExaminationType.MR, "MRI sken" },
            { ExaminationType.ULTRA, "Ultrazvuk" },
            { ExaminationType.EKG, "Elektrokardiogram" },
            { ExaminationType.ECHO, "Ehokardiogram" },
            { ExaminationType.EYE, "Pregled očiju" },
            { ExaminationType.DERM, "Dermatološki pregled" },
            { ExaminationType.DENTA, "Pregled zuba" },
            { ExaminationType.MAMMO, "Mamografija" },
            { ExaminationType.NEURO, "Neurološki pregled" }
        };
    }
}
