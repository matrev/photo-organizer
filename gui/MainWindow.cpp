#include "MainWindow.hpp"
#include "FileInput.hpp"

#include <iostream>
#include <QPushButton>
#include <QVBoxLayout>
#include <QProgressBar>
#include <QMessageBox>
#include <QThread>


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
{
    auto *central = new QWidget(this);
    auto *layout  = new QVBoxLayout(central);

    auto *sourceInput = new FileInput(layout, tr("No source folder selected"));
    auto *destinationInput = new FileInput(layout, tr("No destination folder selected"));
    auto *runBtn = new QPushButton(tr("Start organizing"));
    progressBar = new QProgressBar;
    progressBar->setRange(0, 100);
    progressBar->setValue(0);
    progressBar->hide();

    layout->addWidget(sourceInput);
    layout->addWidget(destinationInput);
    layout->addWidget(runBtn);
    layout->addWidget(progressBar);
    setCentralWidget(central);
    // connect(srcBtn, &FileInput::filePathChanged, this, [this](const QString &newPath) {
    //     sourcePath = newPath;
    // });
    // connect(dstBtn, &FileInput::filePathChanged, this, [this](const QString &newPath) {
    //     destinationPath = newPath;
    // });

    connect(runBtn, &QPushButton::clicked, this, &MainWindow::startWork);
};


void MainWindow::startWork() {
    // if(sourcePath. || destinationPath.isEmpty()) {
    //     QMessageBox::warning(this, tr("Missing paths"), tr("Please select both source and destination folders."));
    //     return;
    // }
    progressBar->show();
    setEnabled(false);
    // Simulate work being done
    for(int i = 0; i <= 100; i += 10) {
        QThread::msleep(100); // Simulate time-consuming task
        progressBar->setValue(i);
    }
    progressBar->hide();
    setEnabled(true);
}