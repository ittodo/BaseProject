{
    "Table": [
        {
            "PC": [
                {
                    "User": {
                        "Type": "I32"
                    }
                },
                {
                    "Level": {
                        "Type": "F32"
                    }
                },
                {
                    "Job":{
                        "Type" : "Reference",
                        "TableName" : "Job"
                    }
                }
            ],
            "Job": [
                {
                    "User": {
                        "Type": "I32"
                    }
                }
            ]
        }
    ]
}