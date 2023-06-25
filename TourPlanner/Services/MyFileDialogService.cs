using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TourPlanner.Services {
    public class MyFileDialogService {
        public string OpenFileDialog() {
            OpenFileDialog fileDialog = new OpenFileDialog {
                Filter = "Json Files (*.json)|*.json",
                Multiselect = false
            };

            fileDialog.ShowDialog();
            
            return fileDialog.FileName;
        }

        public string SaveFileDialog() {
            SaveFileDialog fileDialog = new SaveFileDialog {
                Filter = "Json Files (*.json)|*.json"
            };

            fileDialog.ShowDialog();

            return fileDialog.FileName;
        }
    }
}
