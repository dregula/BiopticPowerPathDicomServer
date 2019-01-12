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
        private string _AccessionNumber;    // (080,050)

        private string _PatientID;          // (100,020)

        private string _Surname;            // Patient name assemmbled into (100,010)

        private string _Forename;

        private string _Title;

        private string _Sex;                // (100,040)

        private System.Nullable<System.DateTime> _DateOfBirth;  // (100,030)

        //note: referring and performing physician names are NOT ^encoded^
        private string _ReferringPhysician; // (080,090)

        private string _PerformingPhysician; // (400,006)

        private string _Modality;           // (80,060)

        private System.Nullable<System.DateTime> _ExamDateAndTime;  // (400,002); Scheduled Procedure Step Start Date

        private string _ExamRoom;           // (400,011) Scheduled Station Name (single: , 010 is array)

        private string _ExamDescription;    // (321,060); Requested Procedure Description ("Study Name")

        private string _StudyUID;           // (0020,000D)

        private string _ProcedureID;        // (401,001); Requested Procedure ID ("Study ID")

        private string _ProcedureStepID;    // (400,009); Scheduled Procedure Step ID

        private string _HospitalName;       // 

        private string _ScheduledAET;       // (400,001); Scheduled Station AE Title

        public ExamsScheduled()
        {
        }

        [Column(Storage = "_AccessionNumber", DbType = "NVarChar(16)")] //SH 
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

        [Column(Storage = "_PatientID", DbType = "NVarChar(64)")]   //LO
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

        [Column(Storage = "_Sex", DbType = "NVarChar(50)")] //CS: 16chars
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

        [Column(Storage = "_ReferringPhysician", DbType = "NVarChar(64)")]  //PN
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

        [Column(Storage = "_PerformingPhysician", DbType = "NVarChar(64)")] //PN: 64chars (including five carets)
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

        [Column(Storage = "_Modality", DbType = "NVarChar(16)")]    //SH
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

        [Column(Storage = "_ExamRoom", DbType = "NVarChar(16)")]    //SH
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

        [Column(Storage = "_ExamDescription", DbType = "NVarChar(64)")] //LO: 64characters
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

        //TRY: 2019-01-12 use the specimen-id, coded for 2D-barcode as the StudyUID
        [Column(Storage = "_StudyUID", DbType = "NVarChar(65)")]    //UI: 64 chars
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

        [Column(Storage = "_ProcedureID", DbType = "NVarChar(16)")] //SH
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

        [Column(Storage = "_ProcedureStepID", DbType = "NVarChar(16)")] //SH
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

        [Column(Storage = "_HospitalName", DbType = "NVarChar(64)")]    //?LO 64
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

        [Column(Storage = "_ScheduledAET", DbType = "NVarChar(16)")]    //SH: 16 characters
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
