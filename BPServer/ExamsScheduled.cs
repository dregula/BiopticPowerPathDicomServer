using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace BiopticPowerPathDicomServer
{
    [Table(Name = "dbo.ExamsScheduled")]
    public partial class ExamsScheduled
    {
        private string _AccessionNumber;

        private string _PatientID;

        private string _Surname;

        private string _Forename;

        private string _Title;

        private string _Sex;

        private System.Nullable<System.DateTime> _DateOfBirth;

        private string _ReferringPhysician;

        private string _PerformingPhysician;

        private string _Modality;

        private System.Nullable<System.DateTime> _ExamDateAndTime;

        private string _ExamRoom;

        private string _ExamDescription;

        private string _StudyUID;

        private string _ProcedureID;

        private string _ProcedureStepID;

        private string _HospitalName;

        private string _ScheduledAET;

        public ExamsScheduled()
        {
        }

        [Column(Storage = "_AccessionNumber", DbType = "NVarChar(50)")]
        public string AccessionNumber
        {
            get { return this._AccessionNumber; }
            set
            {
                if ((this._AccessionNumber != value))
                {
                    this._AccessionNumber = value;
                }
            }
        }

        [Column(Storage = "_PatientID", DbType = "NVarChar(50)")]
        public string PatientID
        {
            get { return this._PatientID; }
            set
            {
                if ((this._PatientID != value))
                {
                    this._PatientID = value;
                }
            }
        }

        [Column(Storage = "_Surname", DbType = "NVarChar(50)")]
        public string Surname
        {
            get { return this._Surname; }
            set
            {
                if ((this._Surname != value))
                {
                    this._Surname = value;
                }
            }
        }

        [Column(Storage = "_Forename", DbType = "NVarChar(50)")]
        public string Forename
        {
            get { return this._Forename; }
            set
            {
                if ((this._Forename != value))
                {
                    this._Forename = value;
                }
            }
        }

        [Column(Storage = "_Title", DbType = "NVarChar(50)")]
        public string Title
        {
            get { return this._Title; }
            set
            {
                if ((this._Title != value))
                {
                    this._Title = value;
                }
            }
        }

        [Column(Storage = "_Sex", DbType = "NVarChar(50)")]
        public string Sex
        {
            get { return this._Sex; }
            set
            {
                if ((this._Sex != value))
                {
                    this._Sex = value;
                }
            }
        }

        [Column(Storage = "_DateOfBirth", DbType = "DateTime")]
        public System.Nullable<System.DateTime> DateOfBirth
        {
            get { return this._DateOfBirth; }
            set
            {
                if ((this._DateOfBirth != value))
                {
                    this._DateOfBirth = value;
                }
            }
        }

        [Column(Storage = "_ReferringPhysician", DbType = "NVarChar(50)")]
        public string ReferringPhysician
        {
            get { return this._ReferringPhysician; }
            set
            {
                if ((this._ReferringPhysician != value))
                {
                    this._ReferringPhysician = value;
                }
            }
        }

        [Column(Storage = "_PerformingPhysician", DbType = "NVarChar(50)")]
        public string PerformingPhysician
        {
            get { return this._PerformingPhysician; }
            set
            {
                if ((this._PerformingPhysician != value))
                {
                    this._PerformingPhysician = value;
                }
            }
        }

        [Column(Storage = "_Modality", DbType = "NVarChar(50)")]
        public string Modality
        {
            get { return this._Modality; }
            set
            {
                if ((this._Modality != value))
                {
                    this._Modality = value;
                }
            }
        }

        [Column(Storage = "_ExamDateAndTime", DbType = "DateTime")]
        public System.Nullable<System.DateTime> ExamDateAndTime
        {
            get { return this._ExamDateAndTime; }
            set
            {
                if ((this._ExamDateAndTime != value))
                {
                    this._ExamDateAndTime = value;
                }
            }
        }

        [Column(Storage = "_ExamRoom", DbType = "NVarChar(50)")]
        public string ExamRoom
        {
            get { return this._ExamRoom; }
            set
            {
                if ((this._ExamRoom != value))
                {
                    this._ExamRoom = value;
                }
            }
        }

        [Column(Storage = "_ExamDescription", DbType = "NVarChar(50)")]
        public string ExamDescription
        {
            get { return this._ExamDescription; }
            set
            {
                if ((this._ExamDescription != value))
                {
                    this._ExamDescription = value;
                }
            }
        }

        [Column(Storage = "_StudyUID", DbType = "NVarChar(65)")]
        public string StudyUID
        {
            get { return this._StudyUID; }
            set
            {
                if ((this._StudyUID != value))
                {
                    this._StudyUID = value;
                }
            }
        }

        [Column(Storage = "_ProcedureID", DbType = "NVarChar(50)")]
        public string ProcedureID
        {
            get { return this._ProcedureID; }
            set
            {
                if ((this._ProcedureID != value))
                {
                    this._ProcedureID = value;
                }
            }
        }

        [Column(Storage = "_ProcedureStepID", DbType = "NVarChar(50)")]
        public string ProcedureStepID
        {
            get { return this._ProcedureStepID; }
            set
            {
                if ((this._ProcedureStepID != value))
                {
                    this._ProcedureStepID = value;
                }
            }
        }

        [Column(Storage = "_HospitalName", DbType = "NVarChar(50)")]
        public string HospitalName
        {
            get { return this._HospitalName; }
            set
            {
                if ((this._HospitalName != value))
                {
                    this._HospitalName = value;
                }
            }
        }

        [Column(Storage = "_ScheduledAET", DbType = "NVarChar(50)")]
        public string ScheduledAET
        {
            get { return this._ScheduledAET; }
            set
            {
                if ((this._ScheduledAET != value))
                {
                    this._ScheduledAET = value;
                }
            }
        }
    }
}
