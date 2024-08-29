/*
 * Code Citations:
 * Citation #54 
 * Citation #56
*/
function validateRequiredFields() {

    /*

     Determines if the user has entered either a zip code OR a city and state value.

    */


    let zip = document.getElementById("zip");
    let city = document.getElementById("city");
    let state = document.getElementById("state");

    let zipVal = document.getElementById("zip").value;
    let cityVal = document.getElementById("city").value;
    let stateVal = document.getElementById("state").value;

    if (cityVal == "" && stateVal == "" && zipVal == "") {

        zip.required = true;
        city.required = true;
        state.required = true;
        
    }

    else if (cityVal == "" && stateVal != "") {

        zip.required = false;
    }

    else if (cityVal != "" && stateVal == "") {

        zip.required = false;
    }

    if (zipVal != "") {

        city.required = false;
        state.required = false;

    }

    else if (cityVal != "" && stateVal != "") {

        zip.required = false;
    }

}
