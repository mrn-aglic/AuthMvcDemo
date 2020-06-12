'use strict';

// TODO: replace _ with # for private methods and fields once bug is Chrome fixed


// FUNKCIJE KOJE ZAPOČINJU DONJOM CRTOM NISU ZA KORIŠTENJE.
// RIJEČ JE O POMOĆNIM FUNKCIJAMA KOJE VAM NE TREBAJU

class Lib {
    constructor() {

    }
}

class Validation {

    _emailRegex() {
        return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    }

    // Provjerava je li email ispravan (ne provjerava broj slova i slično, samo da je oblik ispravan). 
    isValidEmail(email) {
        return _emailRegex().test(email);
    }

    /// Provjerava je li uneseni znak slovo
    isLetter(str) {
        return str.length === 1 && /[a-zA-ZćĆčČđĐšŠžŽ]/i.test(str);
    }

    /// provjerava sadrži li string jedan od posebnih znakova (bez razmaka)
    jedanOdPosebnihZnakova(ch) {
        const reg = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
        return reg.test(ch);
    }

    /// provjerava je li znak ch jednak jednom od:
    /// - prazno mjesto
    /// - tab
    /// - novi red
    /// - carriage return
    /// - vertiakalni tab
    jedanOdPraznihZnakova(ch) {
        return ' \t\n\r\v'.indexOf(ch) > -1;
    }
}

class DomHelper {
    _rowsCreated = 0;
    constructor() {
    }

    _createRow(obj, tag, f) {
        let row = document.createElement('tr');
        for (let prop in obj) {

            const cell = document.createElement(tag);
            cell.innerHTML = f(prop);
            row.appendChild(cell);
        }
        return row;
    }

    _assignRowId(tr) {
        tr.id = `row_${this._rowsCreated}`;
        this._rowsCreated += 1;
    }

    // funkcija za stvaranje retka (tr) koji sadrži ćelije tipa (th)
    // postavlja sadržaj ćelija na ime svojstva objekta.
    // Može se poslati objekt ili klasa kao argument
    // Funkcija vraća tr element (ne dodaje ga u DOM)
    createTableHeader(obj) {
        const o = typeof obj === 'function' ? new obj() : obj;
        return this._createRow(o, 'th', prop => prop);
    }

    // funkcija za stvaranje retka (tr) koji sadrži ćelije tipa (td)
    // postavlja sadržaj ćelija na vrijednost svojstva objekta
    // prima objekt kao argument.
    // Funkcija vraća tr element (ne dodaje ga u DOM)
    createTableRow(obj) {

        function changeArayToString(array) {
            array.toString = function () {
                return `[${this.join(',')}]`;
            };
            return array;
        }
        const row = this._createRow(obj, 'td', prop => Array.isArray(obj[prop]) ?
            changeArayToString(obj[prop]) :
            obj[prop]);
        this._assignRowId(row);
        return row;
    }
}

const domHelper = new DomHelper();
const validation = new Validation();