#ifndef FILEINPUT_HPP
#define FILEINPUT_HPP


#include <QLabel>
#include <QWidget>
#include <QBoxLayout>
#include <QString>


class FileInput : public QWidget {

    public:
        FileInput(QBoxLayout *parent = nullptr, const QString &labelText = "");
        ~FileInput() override = default;
        
        QString filePath;

    private slots:
        void pickFilePath();

    signals:
        void filePathChanged(const QString &newFilePath);

    private:

        QLabel filePathLabel;

};

#endif // MAINWINDOW_HPP