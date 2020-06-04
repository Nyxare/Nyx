"use scrict";
const linksCtrl = {};

const pool = require('../database');
const fetch = require("node-fetch");
var upload = require('express-fileupload');
var docxConverter = require('docx-pdf');
var path = require('path');
var fs = require('fs');

linksCtrl.renderAddDirectory = (req, res) => {
    res.render('list/add');
};

linksCtrl.addDirectory = async (req, res) => {
}

linksCtrl.renderDirectory = async (req, res) => {
    console.log(req.user.userID);
    var list = await pool.query('SELECT * FROM Directories WHERE userID = ?', [req.user.userID]);
    console.log(list)
    res.render('list/list', { list });
}

const extend_pdf = '.pdf';
const extend_docx = '.docx';
var down_name;

linksCtrl.renderUploadFile = async (req, res) => {
    res.render('list/convert');
}

linksCtrl.uploadFile = async (req, res) => {
    console.log(req.files);
    if (req.files.upfile) {
        var file = req.files.upfile,
            name = file.name,
            type = file.mimetype;
        //File where .docx will be downloaded  
        var uploadpath = __dirname + '/uploads/' + name;
        //Name of the file --ex test,example
        const First_name = name.split('.')[0];
        //Name to download the file
        down_name = First_name;
        file.mv(uploadpath, function (err) {
            if (err) {
                console.log(err);
            } else {
                //Path of the downloaded or uploaded file
                var initialPath = path.join(__dirname, `./uploads/${First_name}${extend_docx}`);
                //Path where the converted pdf will be placed/uploaded
                var upload_path = path.join(__dirname, `./uploads/${First_name}${extend_pdf}`);
                //Converter to convert docx to pdf -->docx-pdf is used
                //If you want you can use any other converter
                //For example -- libreoffice-convert or --awesome-unoconv
                docxConverter(initialPath, upload_path, function (err, result) {
                    if (err) {
                        console.log(err);
                    }
                    console.log('result' + result);
                    //res.sendFile(__dirname+'C:\\Users\\Ayano\\Documents\\GitHub\\Nyx\\front-end\\views\\list\\down_html.html')
                    res.download(__dirname + `/uploads/${down_name}${extend_pdf}`, `${down_name}${extend_pdf}`, (err) => {
                        if (err) {
                            res.send(err);
                        } else {
                            //Delete the files from directory after the use
                            console.log('Files deleted');
                            const delete_path_doc = process.cwd() + `/uploads/${down_name}${extend_docx}`;
                            const delete_path_pdf = process.cwd() + `/uploads/${down_name}${extend_pdf}`;
                            try {
                                fs.unlinkSync(delete_path_doc)
                                fs.unlinkSync(delete_path_pdf)
                                //file removed
                            } catch (err) {
                                console.error(err)
                            }
                        }
                    })
                });
            }
        });
    } else {
        res.send("No File selected !");
        res.end();
    }
};

linksCtrl.downloadFile = async (req, res) => {
    //This will be used to download the converted file
    res.download(__dirname + `/uploads/${down_name}${extend_pdf}`, `${down_name}${extend_pdf}`, (err) => {
        if (err) {
            res.send(err);
        } else {
            //Delete the files from directory after the use
            console.log('Files deleted');
            const delete_path_doc = process.cwd() + `/uploads/${down_name}${extend_docx}`;
            const delete_path_pdf = process.cwd() + `/uploads/${down_name}${extend_pdf}`;
            try {
                fs.unlinkSync(delete_path_doc)
                fs.unlinkSync(delete_path_pdf)
                //file removed
            } catch (err) {
                console.error(err)
            }
        }
    })
}

module.exports = linksCtrl;