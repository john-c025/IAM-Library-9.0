using IAM_Library.models.reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.models.geneology
{
    /*
    public class GeneologyMainModel
    {
        public PrimaryGeneologyInfo geneaPrimary { get; set; }
        public LevelOneGeneology level1 { get; set; }
    }

    public class PrimaryGeneologyInfo
    {
        public string idNumber { get; set; }
        public string primaryID { get; set; }
        public string name { get; set; }
        public string sponsorID { get; set; }
        public string uplineID { get; set; }
        public int grp { get; set; }
    }

    public class LevelOneGeneology
    {
        public string left_IDNumber { get; set; }
        public string left_PrimaryID { get; set; }
        public string left_Name { get; set; }
        public string left_SponsorID { get; set; }
        public string left_UplineID { get; set; }
        public int left_Grp { get; set; }

        public Level2Geneology level2Left { get; set; }

        public string right_IDNumber { get; set; }
        public string right_PrimaryID { get; set; }
        public string right_Name { get; set; }
        public string right_SponsorID { get; set; }
        public string right_UplineID { get; set; }
        public int right_Grp { get; set; }

        public Level2Geneology level2Right { get; set; }
    }

    public class Level2Geneology
    {
        public string left_IDNumber { get; set; }
        public string left_PrimaryID { get; set; }
        public string left_Name { get; set; }
        public string left_SponsorID { get; set; }
        public string left_UplineID { get; set; }
        public int left_Grp { get; set; }

        public string right_IDNumber { get; set; }
        public string right_PrimaryID { get; set; }
        public string right_Name { get; set; }
        public string right_SponsorID { get; set; }
        public string right_UplineID { get; set; }
        public int right_Grp { get; set; }
    }
    */

    public class GeneaPrimary
    {
        public string idNumber { get; set; }
        public string primaryID { get; set; }
        public string name { get; set; }
        public string sponsorID { get; set; }
        public string uplineID { get; set; }
        public int grp { get; set; }
    }

    public class LevelOneGeneology
    {
        public string left_IDNumber { get; set; }
        public string left_PrimaryID { get; set; }
        public string left_Name { get; set; }
        public string left_SponsorID { get; set; }
        public string left_UplineID { get; set; }
        public int left_Grp { get; set; }

        public Level2Geneology level2Left { get; set; }

        public string right_IDNumber { get; set; }
        public string right_PrimaryID { get; set; }
        public string right_Name { get; set; }
        public string right_SponsorID { get; set; }
        public string right_UplineID { get; set; }
        public int right_Grp { get; set; }

        public Level2Geneology level2Right { get; set; }
    }

    public class Level2Geneology
    {
        public string left_IDNumber { get; set; }
        public string left_PrimaryID { get; set; }
        public string left_Name { get; set; }
        public string left_SponsorID { get; set; }
        public string left_UplineID { get; set; }
        public int left_Grp { get; set; }

        public string right_IDNumber { get; set; }
        public string right_PrimaryID { get; set; }
        public string right_Name { get; set; }
        public string right_SponsorID { get; set; }
        public string right_UplineID { get; set; }
        public int right_Grp { get; set; }
    }

    public class GeneologyMainModel
    {
        public GeneaPrimary geneaPrimary { get; set; }
        public LevelOneGeneology level1 { get; set; }
    }




    public class GenealogySummary
    {
        public string mainGrp { get; set; }
        public string idNumber { get; set; }
        public string name { get; set; }
        public string sponsorID { get; set; }
        public string uplineID { get; set; }
        public int grp { get; set; }

        List<GenealogySummary> GeneaSummaryList;

        public IEnumerator<GenealogySummary> GetEnumerator()
        {
            foreach (var summary in GeneaSummaryList)
                yield return summary;
        }

    }





    /*
     * 
     * 
     * 
     * {
        "geneaPrimary": {
            "idNumber": "00000001",
            "primaryID": "00000001",
            "name": "iamworldwide01 IAM888 xx iamworldwide01sample",
            "sponsorID": "08072019",
            "uplineID": "08072019",
            "grp": 1
        },
        "level1": {
            "left_IDNumber": "95964700",
            "left_PrimaryID": "95964700",
            "left_Name": "iamworldwide02 am iamworldwide02",
            "left_SponsorID": "00000001",
            "left_UplineID": "00000001",
            "left_Grp": 1,
            "level2Left": {
                "left_IDNumber": "95968002",
                "left_PrimaryID": "95968002",
                "left_Name": "iamworldwide03 am iamworldwide03",
                "left_SponsorID": "00000001",
                "left_UplineID": "95964700",
                "left_Grp": 1,
                "right_IDNumber": "",
                "right_PrimaryID": "",
                "right_Name": "",
                "right_SponsorID": "",
                "right_UplineID": "",
                "right_Grp": 0
            },
            "right_IDNumber": "",
            "right_PrimaryID": "",
            "right_Name": "",
            "right_SponsorID": "",
            "right_UplineID": "",
            "right_Grp": 0,
            "level2Right": {
                "left_IDNumber": "",
                "left_PrimaryID": "",
                "left_Name": "",
                "left_SponsorID": "",
                "left_UplineID": "",
                "left_Grp": 0,
                "right_IDNumber": "",
                "right_PrimaryID": "",
                "right_Name": "",
                "right_SponsorID": "",
                "right_UplineID": "",
                "right_Grp": 0
            }
        }
    }
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */
}
