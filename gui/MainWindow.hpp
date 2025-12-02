#ifndef MAINWINDOW_HPP
#define MAINWINDOW_HPP

#include <QMainWindow>
#include <QProgressBar>


class MainWindow : public QMainWindow {

    public:
        MainWindow(QWidget *parent = nullptr);
        ~MainWindow() override = default;

    private slots:
        void pickSource();

        void pickDestination();

        void startWork();

    private:
        QProgressBar *progressBar{nullptr};
        QString sourcePath;

        QString destinationPath;
};

#endif // MAINWINDOW_HPP