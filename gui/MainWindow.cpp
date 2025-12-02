#include "MainWindow.hpp"
#include <QFileDialog>
#include <QPushButton>
#include <QVBoxLayout>
#include <QProgressBar>


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
{
    auto *central = new QWidget(this);
    auto *layout  = new QVBoxLayout(central);

    auto *srcBtn = new QPushButton(tr("Choose source folder…"));
    auto *dstBtn = new QPushButton(tr("Choose destination folder…"));
    auto *runBtn = new QPushButton(tr("Start organizing"));
    progressBar = new QProgressBar;
    progressBar->setRange(0, 100);
    progressBar->setValue(0);

    layout->addWidget(srcBtn);
    layout->addWidget(dstBtn);
    layout->addWidget(runBtn);
    layout->addWidget(progressBar);
    setCentralWidget(central);

    connect(srcBtn, &QPushButton::clicked, this, &MainWindow::pickSource);
    connect(dstBtn, &QPushButton::clicked, this, &MainWindow::pickDestination);
    // connect(runBtn, &QPushButton::clicked, this, &MainWindow::startWork);
};

void MainWindow::pickSource() {
    sourcePath = QFileDialog::getExistingDirectory(this, tr("Select Source Directory"), "",
                                                QFileDialog::ShowDirsOnly | QFileDialog::DontResolveSymlinks);
};

void MainWindow::pickDestination() {
    destinationPath = QFileDialog::getExistingDirectory(this, tr("Select Destination Directory"), "",
                                                    QFileDialog::ShowDirsOnly | QFileDialog::DontResolveSymlinks);
};