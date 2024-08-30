/*
 * Code Citations:
 * Citation #54 
 * Citation #56
 * Citation #60
*/

function reloadPage() {

    /*
    
    Reloads the page when the user hits the back button.

    */

    // Citation #60
    if (performance.navigation.type === 2) {

        location.reload(true);
    }
}

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

function hideElement(element) {

    /*

    Hides the given element from the webpage.

    */

    element.hideElement = true;

}

function retrieveUSLocation(answer) {

    /*

     Renders the zip code and city/state elements
     for the user, depending on if they want to retrieve
     a US location or not (global).

    */

    let retrieveUSLocationDiv = document.getElementById("getUSLocationPrompt");
    let zipCodeKnow = document.getElementById("knowZipCodePrompt");

    // If the user wants to enter a zip code, unhide the zip code field and data.
    if (answer.toLowerCase() == 'yes') {

        zipCodeKnow.hidden = false;
        let cityStatePrompt = document.getElementById("getCityStatePrompt");
        cityStatePrompt.hidden = true;

    }

    else {

        showCityStatePrompt();
    }

}

function userKnowsZipCode(answer) {

    /*
     
     Renders the zip code data entry field
     for the user or continues to hide it
     if they do not know the zip code.

    */

    let retrieveUSLocationDiv = document.getElementById("getUSLocationPrompt");
    let zipCodeEntry = document.getElementById("getZipCodePrompt");

    // If the user wants to enter a zip code, unhide the zip code field and data.
    if (answer.toLowerCase() == 'yes') {

        zipCodeEntry.hidden = false;
        let cityStatePrompt = document.getElementById("getCityStatePrompt");
        cityStatePrompt.hidden = true;
    }

    else {

        zipCodeEntry.hidden = true;
        showCityStatePrompt();

    }
}

function showCityStatePrompt() {

    /*

     Displays the city/state prompt, usually as
     a result of the user not wanting to search a US
     location or not knowing the zip code.

    */

    let cityStatePrompt = document.getElementById("getCityStatePrompt");
    cityStatePrompt.hidden = false;

}

reloadPage();
