#ifndef MAINWINDOW_HPP
#define MAINWINDOW_HPP

#include "FileInput.hpp"
#include <QMainWindow>
#include <QProgressBar>
#include <QLabel>


class MainWindow : public QMainWindow {

    public:
        MainWindow(QWidget *parent = nullptr);
        ~MainWindow() override = default;

    private slots:
        void pickSource();

        void pickDestination();

        void startWork();

    private:
        QProgressBar *progressBar;
        
        FileInput sourceInput;

        FileInput destinationInput;
};

#endif // MAINWINDOW_HPP