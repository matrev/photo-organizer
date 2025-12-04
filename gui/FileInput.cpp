#include "FileInput.hpp"

#include <QHBoxLayout>
#include <QPushButton>
#include <QFileDialog>
#include <QLabel>

FileInput::FileInput(QBoxLayout *parent, const QString &labelText)
    : filePathLabel(labelText)
{
    auto *layout = new QHBoxLayout(this);
    auto *pickBtn = new QPushButton(tr("Browse…"));
    // layout->setContentsMargins(0, 0, 0, 0);
    // layout->
    layout->addWidget(&filePathLabel);
    layout->addWidget(pickBtn);

    connect(pickBtn, &QPushButton::clicked, this, &FileInput::pickFilePath);
};

void FileInput::pickFilePath() {
    filePath = QFileDialog::getExistingDirectory(this, tr("Select Directory"), "", QFileDialog::ShowDirsOnly | QFileDialog::DontResolveSymlinks);
    filePathLabel.setText(filePath);
    emit filePathChanged(filePath);
};

void FileInput::filePathChanged(const QString &newFilePath) {
    filePath = newFilePath;
    filePathLabel.setText(filePath);
};